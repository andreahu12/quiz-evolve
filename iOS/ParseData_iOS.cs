using System;
using MonoTouch.AVFoundation;
using Xamarin.Forms;
using Parse;
using ProjectEcclesia.iOS;

[assembly: Dependency (typeof (ParseData_iOS))]

public class ParseData_iOS : ParseData {

	public ParseData_iOS() {}
	public void SaveAsync() {

	}

	public void GetQuery(string className) {
		GetQuery (className);
	}

}