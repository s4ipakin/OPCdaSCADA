using System;
using TitaniumAS.Opc.Client.Common;
using System.Collections.Generic;
using TitaniumAS.Opc.Client.Da;
using TitaniumAS.Opc.Client.Da.Browsing;
//using System.Windows.Forms;
using TitaniumAS.Opc.Client;
using System.Globalization;
using System.Linq;

public class OPC_DA_Client
{
	string opcName;
	OpcDaServer server;
	OpcDaGroup group;
	Uri url;
	public delegate void ChangedItems(Dictionary<string, string> itemDict);
	public event ChangedItems ItemsChanged;
	
	public OPC_DA_Client(string opc_Name)
	{		
		opcName = opc_Name;
		
	}
	public OpcDaGroup Group { get { return group; } }
	//public List<string> ItemsValues { get { return itemsValues; } }
	
	public void Connect()
	{
		
		Uri url = UrlBuilder.Build(opcName);
		server = new OpcDaServer(url);
		
		try
		{
			server.Connect();
		}
		catch (Exception ex) { }
		
	}

	public void Disconnect()
	{
		try
		{
			server.Disconnect();
		}
		catch (Exception ex) { }
		
	}
	

	public void CreateGroup(List<string> itemsList)
	{
		Uri url = UrlBuilder.Build(opcName);
		var groupState = new OpcDaGroupState
		{
			Culture = CultureInfo.CurrentCulture, //set LCID for group - some OPC servers may be sensitive for this
			IsActive = true, //only active group can be subscribed
			PercentDeadband = 0.0f, //percent deadband
			TimeBias = TimeSpan.Zero,
			UpdateRate = TimeSpan.FromMilliseconds(1000),
			UserData = "some userdata" //user data
		};

		IList<OpcDaItemDefinition> opcDaItemDefinitions = new List<OpcDaItemDefinition>();
		OpcDaItemDefinition[] definitions = new OpcDaItemDefinition[itemsList.Count];
		for (int i = 0; i < itemsList.Count; i++)
		{
			var definition = new OpcDaItemDefinition
			{
				ItemId = itemsList[i],
				IsActive = true
			};
			definitions[i] = definition;
		}

		if(!server.IsConnected)
		{
			server.Connect();
		}
			
		group = server.AddGroup("MyGroup", groupState);
		group.IsActive = true;
		
		//group.AddItems(definitions);
		OpcDaItemResult[] results = group.AddItems(definitions);
		group.ValuesChanged += Group_ValuesChanged;
	}

	private void Group_ValuesChanged(object sender, OpcDaItemValuesChangedEventArgs e)
	{
		//List<string> itemsList = new List<string>();
		Dictionary<string, string> itemDict = new Dictionary<string, string>();
		foreach (OpcDaItemValue value in e.Values)
		{
			//itemsList.Add(value.Value.ToString());
			try
			{
				itemDict.Add(value.Item.ItemId.ToString(), value.Value.ToString());
			}
			catch(Exception)
			{ }
			
		}
		
		if (ItemsChanged != null)
		{
			ItemsChanged(itemDict);
		}
	}

	public async void WriteValue(string itemId, string itemValue)
	{
		OpcDaItem[] items = { group.Items.FirstOrDefault(i => i.ItemId == itemId) };
		object[] values = { itemValue };
		await group.WriteAsync(items, values);
	}
}

	

