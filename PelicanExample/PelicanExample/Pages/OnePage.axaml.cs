using System;
using Avalonia.Controls;
using Pelican;
using PelicanExample.Utilities;

namespace PelicanExample.Pages; 

public partial class OnePage : DockPanel, IPelicanPage {
	
	protected override Type StyleKeyOverride => typeof(DockPanel); // this is required when subclassing a component for the subclass to work like its parent
	
	public OnePage() {
		InitializeComponent();
	}
}
