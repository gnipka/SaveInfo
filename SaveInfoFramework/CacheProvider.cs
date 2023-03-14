using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SaveInfoFramework
{
    public class CacheProvider
    {
        static byte[] additionalEntropy = { 1, 2, 3, 4, 5 };
        public void CacheConnection(List<ConnectionString> connections)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ConnectionString>));
                MemoryStream memoryStream = new MemoryStream();
                XmlWriter xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xmlSerializer.Serialize(xmlWriter, connections);
                byte[] protectedData = Protect(memoryStream.ToArray());
                File.WriteAllBytes($"{AppDomain.CurrentDomain.BaseDirectory}data.prodected", protectedData);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Serialize data error");
            }
        }

        public List<ConnectionString> GetconnectionFromCache()
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ConnectionString>));
                byte[] protedtedData = File.ReadAllBytes($"{AppDomain.CurrentDomain.BaseDirectory}data.prodected");
                byte[] data = UnProtect(protedtedData);
                return (List<ConnectionString>)xmlSerializer.Deserialize(new MemoryStream(data));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Deserialize data error");
                return null;
            }
        }

        private byte[] Protect(byte[] data)
        {
            try
            {
                return ProtectedData.Protect(data, additionalEntropy, DataProtectionScope.LocalMachine);
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine("Protect error");
                return null;
            }
        }

        private byte[] UnProtect(byte[] data)
        {
            try
            {
                return ProtectedData.Unprotect(data, additionalEntropy, DataProtectionScope.LocalMachine);
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Unprotect error\n {ex.Message}");
                return null;
            }
        }
    }
}
