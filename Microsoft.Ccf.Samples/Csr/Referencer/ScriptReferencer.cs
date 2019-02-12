//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// Feeds appropriate references to the hosted application script 
//===============================================================================

using System;
using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Csr.Referencer.Providers;
using Microsoft.Vsa;

namespace Microsoft.Ccf.Samples.Csr.Referencer
{
	/// <summary>
	/// This class should be used to specify the import dlls that a script needs
	/// </summary>
	public class ScriptReferencer : ReferencerProvider
	{
		/// <summary>
		/// The empty constructor exists for serialization purposes.
		/// </summary>
		public ScriptReferencer()
		{}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="initializer">Not used...</param>
		public ScriptReferencer(string initializer) : base(initializer)
		{}

		/// <summary>
		/// This method loads the import dlls that a script needs to run.
		/// </summary>
		/// <param name="IVsaItems">Reference items added to the .NET script engine.</param>
		public override void LoadAssemblies(IVsaItems items)
		{
				string assemblyName;

				// system.dll
				assemblyName = "system.dll";
				IVsaReferenceItem refItem = (IVsaReferenceItem) items.CreateItem(assemblyName,VsaItemType.Reference,VsaItemFlag.None);
				refItem.AssemblyName = assemblyName;

				// mscorlib.dll
				assemblyName = "mscorlib.dll";
				refItem = (IVsaReferenceItem) items.CreateItem(assemblyName,VsaItemType.Reference,VsaItemFlag.None);
				refItem.AssemblyName = assemblyName;

				// system.windows.forms.dll
				assemblyName = "system.windows.forms.dll";
				refItem = (IVsaReferenceItem) items.CreateItem(assemblyName, VsaItemType.Reference, VsaItemFlag.None);
				refItem.AssemblyName = assemblyName;
		}
	}
}
