import System;
import System.Windows.Forms;
import Microsoft.Office.Interop.Word;
import System.Xml;

class EventHandlers
{
	static function HandleClose()
	{
		// The following is a way to send data back to CCF from the application
		// ExternalApplication.FireRequestAction(new RequestActionEventArgs("Winform App1", "Action31", "data from External App1"));

		MessageBox.Show("Please use Account Application to continue to work with the customer.");
	}
	static function HandleQuit()
	{
		ExternalApplication.Application = null;
	}
}

// Get application object
var app : ApplicationClass = ExternalApplication.Application;

// Add document if it does not already exists
if(app.Documents.Count == 0)
{
	var doc : DocumentClass = app.Documents.Add("Professional Letter.dot");

	// Show the document
	app.Visible = true;

	// Add event handlers
	doc.add_Close(EventHandlers.HandleClose);
	app.add_Quit(EventHandlers.HandleQuit);

	// Enter customer information in the document
	doc.Frames[1].Range.Text = "SampleComm Corporation";
	doc.Frames[2].Range.Text = "123 Main Street\nRedmond, WA 01234";
	
	// Get and parse context string
	var xmldoc : XmlDocument = new XmlDocument();
	xmldoc.LoadXml(ExternalApplication.Context);
	var root : XmlNode = xmldoc.DocumentElement;
	if(root.InnerText.Trim().Length > 0)
	{
		var node : XmlNode = root.SelectSingleNode("descendant::FirstName");
		var address : String = node.InnerText;
		var firstname : String = node.InnerText;
		node = root.SelectSingleNode("descendant::LastName");
		address = address + " " + node.InnerText;
		node = root.SelectSingleNode("descendant::Street");
		address = address + "\n" + node.InnerText;
		node = root.SelectSingleNode("descendant::City");
		address = address + "\n" + node.InnerText;
		node = root.SelectSingleNode("descendant::State");
		address = address + ", " + node.InnerText;
		node = root.SelectSingleNode("descendant::ZipCode");
		address = address + " " + node.InnerText + "\n";
		
		doc.Fields[2].Code.Text = "MacroButton NoMacro";
		doc.Fields[2].Result.Text = address;
		doc.Fields[2].Update();
		
		doc.Fields[3].Result.Text = "Dear " + firstname + ":"
	}
	
	doc.Fields[6].Code.Text = "MacroButton NoMacro";
	doc.Fields[6].Result.Text = "John Doe";
	doc.Fields[6].Update();
	
	doc.Fields[7].Code.Text = "MacroButton NoMacro";
	doc.Fields[7].Result.Text = "Customer Service Representative";
	doc.Fields[7].Update();

	doc.Fields[4].Select();
}
else
{
	MessageBox.Show("You must save and close open documentations before composing a letter to customer");
}