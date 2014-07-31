using System;
using Xamarin.Forms;
using ProjectEcclesia.Android;
using System.Collections.Generic;

/**
 * Allows for use of parse in Android.
 * */

[assembly: Dependency (typeof(ParseData_Android))]

public class ParseData_Android : Java.Lang.Object, ParseData {

	public ParseData_Android () {
	}

	public void SaveAsync() {
	}

	public void GetAsync() {
	}

	void FetchAsync() {
	}
}