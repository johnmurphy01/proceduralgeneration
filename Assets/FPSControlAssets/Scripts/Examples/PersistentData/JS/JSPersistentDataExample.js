//import UnityEngine;
import FPSControl.Data;

#pragma strict

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

function Start ()
{    
    var nameSpace : String = "JS_TEST"; // The namespace is the name of the .txt file that your data will be saved into
    var identifier : String = "Test1"; // A namespace can contain multiple instances of the data type, this helps differenciate them.

    var data : JSDataExample;

    if(PersistentData.Exists.<JSDataExample>(nameSpace,identifier))
    {
        Debug.Log("Found Persistent Data for " + nameSpace + "." + identifier);

        // Load the object
        data = PersistentData.Read.<JSDataExample>(nameSpace,identifier);

        // Read out the values to the console
        Debug.Log(nameSpace + "." + identifier + ".MyString: " + data.MyString);
        Debug.Log(nameSpace + "." + identifier + ".MyFloat: " + data.MyFloat);
    }
    else // It doesn't exist, so create a default.
    {
        // Create a JSDataExample object
        data = new JSDataExample();
        data.MyString = "Pi";
        data.MyFloat = 3.14;

        // Write it to Persistent Data
        PersistentData.Write.<JSDataExample>(nameSpace,identifier,data,true);// In almost every case, append should be TRUE
    }
}