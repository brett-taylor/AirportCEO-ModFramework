using Harmony;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ACMH.Utilities.Misc
{
    [HarmonyPatch(typeof(PlayerInputController))]
    [HarmonyPatch("Update")]
    public static class UnityObjectDumpFieldsHotKey
    {
        public static void Postfix()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Y))
            {
                GameObject gameObject8 = Singleton<TrafficController>.Instance.SpawnVehicleGameObject(Enums.VehicleType.ServiceCar, Enums.VehicleSubType.Unspecified);
                gameObject8.GetComponent<ServiceCarController>().Initialize();
                gameObject8.DumpFields();
                Logger.ShowDialog("Worked.");
            }
        }
    }

    public static class UnityObjectDumpFields
    {
        private static readonly string DUMPS_FOLDER_NAME = "Dumps";

        public static void DumpFields(this GameObject mb)
        {
            string path = Path.Combine(Path.GetDirectoryName(ACMH.Mod.Assembly.Location), DUMPS_FOLDER_NAME);
            Directory.CreateDirectory(path);
            string fileLocation = Path.Combine(path, $"{mb.name}.txt");
            TextWriter stream = new StreamWriter(fileLocation, false);

            foreach (Component c in mb.GetComponentsInChildren<Component>())
            {
                stream.WriteLine($"Component {c.name} || Type {c.GetType()} || Parent {c.transform.parent?.name}");
                DumpComponentToStream(c, stream);
                stream.WriteLine(" ");
            }

            stream.Close();
        }

        private static void DumpComponentToStream(Component c, TextWriter stream)
        {
            // Airport CEO defined MonoBehaviours can use reflection to print out the information.
            // Unity components are special edge cases where reflection returns 0 fields.
            if (c is Transform)
                DumpComponentToStream((Transform) c, stream);
            else if (c is Light)
                DumpComponentToStream((Light) c, stream);
            else if (c is SpriteRenderer)
                DumpComponentToStream((SpriteRenderer) c, stream);
            else
                DumpComponentToStreamFallback(c, stream);
        }

        private static void DumpComponentToStreamFallback(Component c, TextWriter stream)
        {
            List<string> fieldNames = c.GetType().GetFields().Select(field => field.Name).ToList();
            List<object> fieldValues = c.GetType().GetFields().Select(field => field.GetValue(c)).ToList();
            for (int i = 0; i < fieldNames.Count; i++)
                stream.WriteLine($"{fieldNames[i]} : {fieldValues[i]}");
        }

        private static void DumpComponentToStream(Transform t, TextWriter stream)
        {
            stream.WriteLine($"position: {t.position}");
            stream.WriteLine($"localPosition: {t.localPosition}");
            stream.WriteLine($"rotation: {t.rotation}");
            stream.WriteLine($"localRotation: {t.localRotation}");
            stream.WriteLine($"eulerAngles: {t.eulerAngles}");
            stream.WriteLine($"localEulerAngles: {t.localEulerAngles}");
            stream.WriteLine($"lossyScale: {t.lossyScale}");
            stream.WriteLine($"localScale: {t.localScale}");
        }

        private static void DumpComponentToStream(Light t, TextWriter stream)
        {
            stream.WriteLine($"type: {t.type}");
            stream.WriteLine($"color: {t.color}");
            stream.WriteLine($"colorTemperature: {t.colorTemperature}");
            stream.WriteLine($"intensity: {t.intensity}");
            stream.WriteLine($"bounceIntensity: {t.bounceIntensity}");
            stream.WriteLine($"renderMode: {t.renderMode}");
            stream.WriteLine($"range: {t.range}");
            stream.WriteLine($"spotAngle: {t.spotAngle}");
        }

        private static void DumpComponentToStream(SpriteRenderer t, TextWriter stream)
        {
            stream.WriteLine($"color: {t.color}");
            stream.WriteLine($"flipX: {t.flipX}");
            stream.WriteLine($"flipY: {t.flipY}");
            stream.WriteLine($"material: {t.material.name}");
            stream.WriteLine($"drawMode: {t.drawMode}");
            stream.WriteLine($"sortingLayerID: {t.sortingLayerID}");
            stream.WriteLine($"sortingLayerName: {t.sortingLayerName}");
            stream.WriteLine($"sortingOrder: {t.sortingOrder}");
            stream.WriteLine($"maskInteraction: {t.maskInteraction}");
            stream.WriteLine(" ");
            stream.WriteLine($"Sprite: {t.sprite?.name}"); 
            stream.WriteLine($"Texture: {t.sprite?.texture?.name}");
            stream.WriteLine($"Texture: {t.sprite?.texture?.name}");
        }
    }
}
