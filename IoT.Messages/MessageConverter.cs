using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IoT.Messages
{
    public static class MessageConverter
    {
        public static byte[] Serialize<T>(T message)
        {
            string strValue = JsonConvert.SerializeObject(message, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return Encoding.UTF8.GetBytes(strValue);
        }

        public static T Deserialize<T>(byte[] message)
        {
            var strValue = Encoding.UTF8.GetString(message);
            var msg = JsonConvert.DeserializeObject<T>(
                strValue,
                new JsonSerializerSettings() { 
                    ContractResolver = new CamelCasePropertyNamesContractResolver() 
            });
            return msg;
        }
    }
}
