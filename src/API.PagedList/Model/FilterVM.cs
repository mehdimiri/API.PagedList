namespace API.PagedList.Model;

public class FilterVM
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public List<WhereVM> Conditions { get; set; }
    public OrderVM OrderBy { get; set; }
}

public class WhereVM
{
    public string Name { get; set; }
    public object Value { get; set; }
    public string Comparison { get; set; }
}

public class OrderVM
{
    public string Name { get; set; }
    public bool Ascending { get; set; }
}
