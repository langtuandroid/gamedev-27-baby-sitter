using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MenuItems   {
	public static Dictionary <string,MenuItemData> mitd = new Dictionary<string, MenuItemData>();
	public static bool BAllItemsUnlocked = false;
	
	public   MenuItems()
	{
		
		if( mitd.Count == 0)
		{
			
			//MG1 - PRESVLACENJE BEBE
			//pelene
			mitd.Add("M01_01",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "diaper_1" ,  ImgeSize = new Vector2(220,140), Locked = false });
			mitd.Add("M01_02",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "diaper_2",   ImgeSize = new Vector2(220,140), Locked = true });
			mitd.Add("M01_03",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "diaper_3",   ImgeSize = new Vector2(220,140), Locked = false });
			mitd.Add("M01_04",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "diaper_4",   ImgeSize = new Vector2(220,140), Locked = true });
			mitd.Add("M01_05",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "diaper_5",   ImgeSize = new Vector2(220,140), Locked = false });
			mitd.Add("M01_06",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "diaper_6" ,  ImgeSize = new Vector2(220,140), Locked = true });
			mitd.Add("M01_07",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "diaper_7",   ImgeSize = new Vector2(220,140), Locked = false });
			mitd.Add("M01_08",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "diaper_8",   ImgeSize = new Vector2(220,140), Locked = true });
			mitd.Add("M01_09",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "diaper_9",   ImgeSize = new Vector2(220,140), Locked = false });
			mitd.Add("M01_10",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "diaper_10",   ImgeSize = new Vector2(220,140), Locked = true });
			mitd.Add("M01_11",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "diaper_11" ,  ImgeSize = new Vector2(220,140), Locked = false });
			mitd.Add("M01_12",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "diaper_12",   ImgeSize = new Vector2(220,140), Locked = true });
			
			//benkice
			mitd.Add("M02_01",new MenuItemData {ButtonImgName = "", Atlas = "Onesie", Name = "1" ,  ImgeSize = new Vector2(372,500), Locked = false });
			mitd.Add("M02_02",new MenuItemData {ButtonImgName = "", Atlas = "Onesie", Name = "2",   ImgeSize = new Vector2(372,500), Locked = true });  
			mitd.Add("M02_03",new MenuItemData {ButtonImgName = "", Atlas = "Onesie", Name = "3",   ImgeSize = new Vector2(372,500), Locked = false });  
			mitd.Add("M02_04",new MenuItemData {ButtonImgName = "", Atlas = "Onesie", Name = "4",   ImgeSize = new Vector2(372,500), Locked = true });
			mitd.Add("M02_05",new MenuItemData {ButtonImgName = "", Atlas = "Onesie", Name = "5",   ImgeSize = new Vector2(372,500), Locked = false });  
			mitd.Add("M02_06",new MenuItemData {ButtonImgName = "", Atlas = "Onesie", Name = "6" ,  ImgeSize = new Vector2(372,500), Locked = true });
			mitd.Add("M02_07",new MenuItemData {ButtonImgName = "", Atlas = "Onesie", Name = "7",   ImgeSize = new Vector2(372,500), Locked = false });  
			mitd.Add("M02_08",new MenuItemData {ButtonImgName = "", Atlas = "Onesie", Name = "8",   ImgeSize = new Vector2(372,500), Locked = true });  
			mitd.Add("M02_09",new MenuItemData {ButtonImgName = "", Atlas = "Onesie", Name = "9",   ImgeSize = new Vector2(372,500), Locked = false });
			mitd.Add("M02_10",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "10",   ImgeSize = new Vector2(372,500), Locked = true }); 
			
			
			//carapice
			mitd.Add("M03_01",new MenuItemData {ButtonImgName = "diaper-2/icon_socks_1", Atlas = "diaper", Name = "socks_1" ,  ImgeSize = new Vector2(375,140), Locked = false });
			mitd.Add("M03_02",new MenuItemData {ButtonImgName = "diaper-2/icon_socks_2", Atlas = "diaper", Name = "socks_2",   ImgeSize = new Vector2(375,140), Locked = true });  
			mitd.Add("M03_03",new MenuItemData {ButtonImgName = "diaper-2/icon_socks_3", Atlas = "diaper", Name = "socks_3",   ImgeSize = new Vector2(375,140), Locked = false });  
			mitd.Add("M03_04",new MenuItemData {ButtonImgName = "diaper-2/icon_socks_4", Atlas = "diaper-2", Name = "socks_4",   ImgeSize = new Vector2(375,140), Locked = true });
			mitd.Add("M03_05",new MenuItemData {ButtonImgName = "diaper-2/icon_socks_5", Atlas = "diaper-2", Name = "socks_5",   ImgeSize = new Vector2(375,140), Locked = false });  
			mitd.Add("M03_06",new MenuItemData {ButtonImgName = "diaper-2/icon_socks_6", Atlas = "diaper-2", Name = "socks_6" ,  ImgeSize = new Vector2(375,140), Locked = true });
			mitd.Add("M03_07",new MenuItemData {ButtonImgName = "diaper-2/icon_socks_7", Atlas = "diaper-2", Name = "socks_7",   ImgeSize = new Vector2(375,140), Locked = false });  
			mitd.Add("M03_08",new MenuItemData {ButtonImgName = "diaper-2/icon_socks_8", Atlas = "diaper-2", Name = "socks_8",   ImgeSize = new Vector2(375,140), Locked = true });  
			mitd.Add("M03_09",new MenuItemData {ButtonImgName = "diaper-2/icon_socks_9", Atlas = "diaper-2", Name = "socks_9",   ImgeSize = new Vector2(375,140), Locked = false });
			mitd.Add("M03_10",new MenuItemData {ButtonImgName = "diaper-2/icon_socks_10", Atlas = "diaper-2", Name = "socks_10",   ImgeSize = new Vector2(375,140), Locked = true }); 
			
			//portikla
			mitd.Add("M04_01",new MenuItemData {ButtonImgName = "", Atlas = "diaper-2", Name = "bib_1" ,  ImgeSize = new Vector2(200,200), Locked = false });
			mitd.Add("M04_02",new MenuItemData {ButtonImgName = "", Atlas = "diaper-2", Name = "bib_2",   ImgeSize = new Vector2(200,200), Locked = true });  
			mitd.Add("M04_03",new MenuItemData {ButtonImgName = "", Atlas = "diaper-2", Name = "bib_3",   ImgeSize = new Vector2(200,200), Locked = false });  
			mitd.Add("M04_04",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "bib_4",   ImgeSize = new Vector2(200,200), Locked = true });
			mitd.Add("M04_05",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "bib_5",   ImgeSize = new Vector2(200,200), Locked = false });  
			mitd.Add("M04_06",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "bib_6" ,  ImgeSize = new Vector2(200,200), Locked = true });
			mitd.Add("M04_07",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "bib_7",   ImgeSize = new Vector2(200,200), Locked = false });  
			mitd.Add("M04_08",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "bib_8",   ImgeSize = new Vector2(200,200), Locked = true });  
			mitd.Add("M04_09",new MenuItemData {ButtonImgName = "", Atlas = "diaper", Name = "bib_9",   ImgeSize = new Vector2(200,200), Locked = false });



			//MG2 - KUPANJE BEBE - IGRACKE ZA KADU
			mitd.Add("M05_01",new MenuItemData { Locked = false });
			mitd.Add("M05_02",new MenuItemData { Locked = true });
			mitd.Add("M05_03",new MenuItemData { Locked = false });
			mitd.Add("M05_04",new MenuItemData { Locked = true });
			mitd.Add("M05_05",new MenuItemData { Locked = false });
			mitd.Add("M05_06",new MenuItemData { Locked = true });
			mitd.Add("M05_07",new MenuItemData { Locked = false });
			mitd.Add("M05_08",new MenuItemData { Locked = true });
			 

			//MG3 - USPAVLJIVANJE BEBE 
			mitd.Add("M06_01",new MenuItemData { Locked = false });
			mitd.Add("M06_02",new MenuItemData { Locked = false });
			mitd.Add("M06_03",new MenuItemData { Locked = true });
			mitd.Add("M06_04",new MenuItemData { Locked = true });
			mitd.Add("M06_05",new MenuItemData { Locked = false });
			mitd.Add("M06_06",new MenuItemData { Locked = false });
			mitd.Add("M06_07",new MenuItemData { Locked = true });
			mitd.Add("M06_08",new MenuItemData { Locked = true });
			mitd.Add("M06_09",new MenuItemData { Locked = false });
			mitd.Add("M06_10",new MenuItemData { Locked = false });
			mitd.Add("M06_11",new MenuItemData { Locked = true });
			mitd.Add("M06_12",new MenuItemData { Locked = true });
			mitd.Add("M06_13",new MenuItemData { Locked = false });
			mitd.Add("M06_14",new MenuItemData { Locked = false });
			mitd.Add("M06_15",new MenuItemData { Locked = true });
			mitd.Add("M06_16",new MenuItemData { Locked = true });

			//MG8 - PUSTANJE PLOCA
			mitd.Add("M07_01",new MenuItemData { Locked = false });
			mitd.Add("M07_02",new MenuItemData { Locked = true });
			mitd.Add("M07_03",new MenuItemData { Locked = false });
			mitd.Add("M07_04",new MenuItemData { Locked = true });
			mitd.Add("M07_05",new MenuItemData { Locked = false });

		}

	}


	public static void UnlockAll()
	{
		foreach(KeyValuePair<string,MenuItemData>  kvp in mitd)
		{
			kvp.Value.Locked = false;
		}
		
	}


	public  Dictionary <string,MenuItemData>  ReturmMenu(int menu)
	{
		Dictionary <string,MenuItemData> m = new Dictionary<string, MenuItemData>();
		string test = "M"+menu.ToString().PadLeft(2,'0')+"_";
		foreach(  KeyValuePair<string,MenuItemData>  kvp in mitd)
		{
			if( kvp.Key.StartsWith (test))
			{
				m.Add(kvp.Key,kvp.Value);
			}
		}
		return m;
	}

	public void ReturnMenuImages( int menu, out Sprite[] images, int images_length )
	{
		images = new Sprite[images_length];
		 
		string  _atlasList = "";
		List<string> atlasList = new List<string>();
		List<string> spriteList = new List<string>();
		for(int i = 1; i<=images_length;i++)
		{
			string test = "M"+menu.ToString().PadLeft(2,'0')+"_" + i.ToString().PadLeft(2,'0');
			//Debug.Log( test );
			if(!_atlasList.Contains ("^"+  mitd[test].Atlas + ";"))
			{
				_atlasList+= ("^"+  mitd[test].Atlas + ";");
				atlasList.Add(mitd[test].Atlas); 
			}
			spriteList.Add(mitd[test].Name);
		}
		
		foreach(string atlas in atlasList)
		{
			foreach( Sprite spr in Resources.LoadAll<Sprite>("MenuItems/"+atlas))
			{
				for(int j = 0; j<images_length; j++)
				{
					if( spr.name == spriteList[j])  
					{
						images[j] = spr;
					}
				}
			}
		}
	}
	
	 
}

public class MenuItemData
{
	public string ButtonImgName = "";
	public string  Atlas = "";
	public string Name = "";
	
	public bool Locked = false;
	public Vector2 ImgeSize = new Vector2(20,20);
}
