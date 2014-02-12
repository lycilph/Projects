using AmazonTest.AmazonService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AmazonTest
{
    /*
     * Email address:     lycilph@gmail.com
     * Access Key ID:     AKIAJ2LLLQOAIUXRUBFA
     * Secret Access Key: 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
     */

    /*
     * http://www.codeproject.com/Articles/37618/Hello-Amazon-Making-a-first-request-to-the-Amazon
     * http://flyingpies.wordpress.com/2009/08/01/17/
     */

    class Program
    {
        private static string access_key_id = "AKIAJ2LLLQOAIUXRUBFA";
        private static string secret_access_key = "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";
        private static string associate_tag = "my_associate_tag";

        static void Main(string[] args)
        {
            // Make the client
            AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient("AWSECommerceServicePort");
            client.ChannelFactory.Endpoint.EndpointBehaviors.Add(new AmazonSigningEndpointBehavior(access_key_id, secret_access_key));

            //var versions = GetAlternateVersions(client, "0441013074");
            //SaveObject(versions, "alternate_versions.txt");

            //GetVariations(client, "0441013074");

            var similar = SimilarityLookup(client, "0441013074");
            SaveObject(similar, "similar_0441013074.txt");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static IEnumerable<ItemAlternateVersion> GetAlternateVersions(AWSECommerceServicePortTypeClient client, string asin)
        {
            ItemLookupRequest request = new ItemLookupRequest();
            request.IdType = ItemLookupRequestIdType.ASIN;
            request.ItemId = new string[] { asin };
            request.ResponseGroup = new string[] { "AlternateVersions" };

            ItemLookup lookup = new ItemLookup();
            lookup.AWSAccessKeyId = access_key_id;
            lookup.AssociateTag = associate_tag;
            lookup.Request = new ItemLookupRequest[] { request };

            var response = client.ItemLookup(lookup);

            if (response.Items != null && response.Items.Count() > 0)
            {
                var items = response.Items[0];
                if (items.Item != null && items.Item.Count() > 0)
                {
                    var item = items.Item[0];
                    if (item.AlternateVersions != null && item.AlternateVersions.Count() > 0)
                        return item.AlternateVersions;
                }
            }
            return null;
        }
        private static void GetVariations(AWSECommerceServicePortTypeClient client, string asin)
        {
            ItemLookupRequest request = new ItemLookupRequest();
            request.IdType = ItemLookupRequestIdType.ASIN;
            request.ItemId = new string[] { asin };
            request.ResponseGroup = new string[] { "Variations" };

            ItemLookup lookup = new ItemLookup();
            lookup.AWSAccessKeyId = access_key_id;
            lookup.AssociateTag = associate_tag;
            lookup.Request = new ItemLookupRequest[] { request };

            var response = client.ItemLookup(lookup);
        }

        private static SimilarityLookupResponse SimilarityLookup(AWSECommerceServicePortTypeClient client, string asin)
        {
            var request = new SimilarityLookupRequest();
            request.ItemId = new string[] { asin };
            request.ResponseGroup = new string[] { "Large", "ItemAttributes" };

            var lookup = new SimilarityLookup();
            lookup.AWSAccessKeyId = access_key_id;
            lookup.AssociateTag = associate_tag;
            lookup.Request = new SimilarityLookupRequest[] { request };

            var response = client.SimilarityLookup(lookup);
            return response;
        }
        private static ItemLookupResponse ItemLookup(AWSECommerceServicePortTypeClient client, string asin)
        {
            ItemLookupRequest request = new ItemLookupRequest();
            request.IdType = ItemLookupRequestIdType.ASIN;
            request.ItemId = new string[] { asin };
            request.ResponseGroup = new string[] { "Large", "ItemAttributes", "AlternateVersions", "Variations" };

            ItemLookup lookup = new ItemLookup();
            lookup.AWSAccessKeyId = access_key_id;
            lookup.AssociateTag = associate_tag;
            lookup.Request = new ItemLookupRequest[] { request };
            
            var response = client.ItemLookup(lookup);
            return response;
        }

        private static void SaveObject(object obj, string filename)
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var filepath = Path.Combine(Path.GetDirectoryName(location), filename);

            using (var fs = File.Open(filename, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                sw.Write(json);
            }
        }

        private static void ItemSearch()
        {
            // Create the request object
            ItemSearchRequest request = new ItemSearchRequest();

            // Fill request object with request parameters
            request.ResponseGroup = new string[] { "Large", "ItemAttributes", "AlternateVersions" };

            // Set SearchIndex to All and use the scanned EAN
            // as the keyword, this should generate a single response 
            request.SearchIndex = "Books";
            request.Keywords = "Century rain"; // asin = 0441013074
            request.Author = "Alastair Reynolds";
            
            // Make the item search 
            ItemSearch search = new ItemSearch();

            // It is ABSOLUTELY CRITICAL that you change
            // the AWSAccessKeyID to YOUR uniqe value
            // Signup for an account (and AccessKeyID) at http://aws.amazon.com/ 
            search.AWSAccessKeyId = access_key_id;
            search.AssociateTag = associate_tag;

            // Set the request on the search wrapper - multiple requests
            // can be submitted on one search
            search.Request = new ItemSearchRequest[] { request };

            // Make the port
            AWSECommerceServicePortTypeClient port = new AWSECommerceServicePortTypeClient("AWSECommerceServicePort");
            port.ChannelFactory.Endpoint.EndpointBehaviors.Add(new AmazonSigningEndpointBehavior(access_key_id, secret_access_key));

            //Send the request, store the response and display some of the results
            ItemSearchResponse response = port.ItemSearch(search);

            foreach (var items in response.Items)
            {
                foreach (var item in items.Item)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", item.ItemAttributes.Title, item.ASIN, item.ItemAttributes.Author[0]);

                    if (item.AlternateVersions != null)
                    {
                        Console.WriteLine(" - Alternate versions");
                        foreach (var version in item.AlternateVersions)
                        {
                            Console.WriteLine(" -- \t{0}\t{1}\t{2}", version.Title, version.Binding, version.ASIN);
                        }
                    }
                }
            }
        }
    }
}
