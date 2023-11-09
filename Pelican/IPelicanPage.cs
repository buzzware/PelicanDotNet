namespace Pelican; 

public interface IPelicanPage {

	public object? Content { get; set; }
	public object? DataContext { get; set; }
	
}
