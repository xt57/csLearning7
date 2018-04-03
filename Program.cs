
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace HttpClientSample
{

class
TdLogin
{

	static
	void
	Main()	{
		
		RunAsync().GetAwaiter().GetResult();
	}

	static
	async
	Task RunAsync()	{

		String					sUID		=	"";
		String					sPWD		=	"";
		String					sSOURCE		=	"";
		String					sVERSION	=	"";

		HttpClient 				myClient;
		FormUrlEncodedContent	myContent;
		String 					sXmlText	= "";
		XDocument 				xmlDoc;
		String 					sUri;
		int 					i;
		
		try {

			myClient = new HttpClient();

			myClient.BaseAddress = new Uri("https://apis.tdameritrade.com");
			myClient.DefaultRequestHeaders.Accept.Clear();
			myClient.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue("application/xml"));

			sUri =	"/apps/100/LogIn?source="	+	Uri.EscapeDataString (	sSOURCE		) +
					"&version=" 				+	Uri.EscapeDataString (	sVERSION	);

			var d = new Dictionary<string, string>();
			d.Add(	"userid", 	sUID		);
			d.Add(	"password",	sPWD		);
			d.Add(	"source",	sSOURCE		);
			d.Add(	"version",	sVERSION	);

			myContent = new FormUrlEncodedContent (d);

			HttpResponseMessage myResponse;

			myResponse = await myClient.PostAsync(	sUri,	myContent);

			myResponse.EnsureSuccessStatusCode();

			if ( !	myResponse.IsSuccessStatusCode)	{
				
				Console.WriteLine( "response not retrieved successfully" );
				Environment.Exit( 9);
			}

			sXmlText = await myResponse.Content.ReadAsStringAsync();

			xmlDoc = XDocument.Parse( sXmlText);
			
			Console.WriteLine("press enter to continue on to XML");
			Console.ReadLine();

			Console.WriteLine("Reading with XmlReader");

			String result = xmlDoc.Root.Element ("result").Value;

			var node = xmlDoc.Root.Element ("xml-log-in");

			Console.WriteLine (	"user-id    : {0}", 	node.Element ("user-id").Value					);
			Console.WriteLine (	"session-id : {0}", 	node.Element ("session-id").Value				);			
			Console.WriteLine (	"assoc acct : {0}", 	node.Element ("associated-account-id").Value	);	
			
			i = 1;	i += 1;

			//	TimeSpan	tsTimeout	=
			//	TimeSpan.FromMinutes (int.Parse (node.Element ("timeout").Value));
		}
		catch (Exception e)		{
			Console.WriteLine(e.Message);
		}

		Console.WriteLine("press enter to end");
		Console.ReadLine();

		//	if (this.IsAuthenticated = xml.Root.Element ("result").Value == "OK") {
	}

}//class

}//namespace

/*

    [XmlRoot("suggest")]
public class DeliciousSuggest {
    [XmlElement("popular")]
    public string[] Popular { get; set; }

    [XmlElement("recommended")]
    public string[] Recommended { get; set; }

    [XmlElement("network")]
    public string[] Network { get; set; }
}
and use XmlSerializer to deserialize.

You should read the response back from del.icio.us as a string, and then you can deserialize it as follows:

var s = "this is the response from del"; 
var buffer = Encoding.UTF8.GetBytes(s); 
using (var stream = new MemoryStream(buffer)) { 
    var serializer = new XmlSerializer(typeof(DeliciousSuggest)); 
    var deliciousSuggest = (DeliciousSuggest)serializer.Deserialize(stream); 
    //then do whatever you want
}




*/
