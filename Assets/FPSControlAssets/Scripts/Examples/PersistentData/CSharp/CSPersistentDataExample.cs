using UnityEngine;
using System.Collections;
using FPSControl.Data;

/*
    - The premise of PersistentData is very simple. An object can be serialized into a JSON-formatted .txt file. 
    The .txt file is saved to Unity's default Application.persistentDataPath
    
    - When recording PersistentData, it must be provided with a String namespace and a String identifier.
    The namespace is a unique name for that object's .txt
    The identifier serves as a way to differenciate different data instances - i.e. save files, stats for different characters, etc.
    A namespace can contain multiple instances of data in a collection, it's best to think of the identifier like a key to the value of your data
    and your namespace is like a .NET Dictionary.

    - All reading/writing must be done through a Generics inteface, denoted by the carrots '<' & '>', as shown below.
*/

public class CSPersistentDataExample : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        string nameSpace = "CS_Test"; // The namespace is the name of the .txt file that your data will be saved into
        string identifier = "Test1"; // A namespace can contain multiple instances of the data type, this helps differenciate them.

        CSDataExample data;

        if (PersistentData.Exists<CSDataExample>(nameSpace, identifier))
        {
            Debug.Log("Found Persistent Data for " + nameSpace + "." + identifier);

            // Load the object
            data = PersistentData.Read<CSDataExample>(nameSpace,identifier);

            // Read out the values to the console
            Debug.Log(nameSpace + "." + identifier + ".myString: " + data.myString);
            Debug.Log(nameSpace + "." + identifier + ".myFloat: " + data.myFloat);
        }
        else // It doesn't exist, so create a default.
        {
            // Create a CSDataExample object
            data = new CSDataExample();
            data.myString = "Pi";
            data.myFloat = 3.14F;

            // Write it to Persistent Data
            PersistentData.Write<CSDataExample>(nameSpace,identifier,data,true);
        }
	
	}
}
