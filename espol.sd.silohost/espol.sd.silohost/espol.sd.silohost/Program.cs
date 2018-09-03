using System;
using System.Data;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace espol.sd.silohost
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // First, configure and start a local silo
            var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
            var silo = new SiloHost("TestSilo", siloConfig);
            silo.InitializeOrleansSilo();
            silo.StartOrleansSilo();

            Console.WriteLine("Silo started.");

            // Then configure and connect a client.
            var clientConfig = ClientConfiguration.LocalhostSilo();
            var client = new ClientBuilder().UseConfiguration(clientConfig).Build();
            client.Connect().Wait();

            Console.WriteLine("Client connected.");

            GrainClient.Initialize(ClientConfiguration.LocalhostSilo());
            var grain = GrainClient.GrainFactory.GetGrain<IGIFSGrain>(1);

            //Se define el objeto objRemoto para referenciar el grain de la interfaz IGIFSGrain del SILO
            //IGIFSGrain objRemoto = client.GetGrain<IGIFSGrain>(1);

            //Definimos el DataSet oDS y registramos en el mismo, el DataSet de noticias de la cache, que se 
            //obtiene del método "LeerNoticias", declarado en el SILO

            grain.CargarGIFSALaCache().Wait();
            DataSet oDS = grain.LeerGIFSDesdeLaCache().Result;

            Console.WriteLine(oDS.Tables.Count.ToString());

            //
            // This is the place for your test code.
            //

            Console.WriteLine("\nPress Enter to terminate...");
            Console.ReadLine();

            // Shut down
            client.Close();
            silo.ShutdownOrleansSilo();
        }
    }
}
