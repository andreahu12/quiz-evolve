using System;
using Xamarin.Forms;

/**
 * Parent of ParseData_iOS and ParseData_Android
 * Allows for usage of Parse in Forms.
 * */
public interface ParseData {
	void SaveAsync();
}