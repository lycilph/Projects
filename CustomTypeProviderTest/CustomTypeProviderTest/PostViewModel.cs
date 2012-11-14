using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomTypeProviderTest
{
  public class PostViewModel : ICustomTypeProvider, INotifyPropertyChanged
  {
    private Post internal_post;

    public PostViewModel(Post post)
    {
      internal_post = post;

      // Forward all notifications from internal object
      internal_post.PropertyChanged += (sender, args) => NotifyPropertyChanged(args.PropertyName);

      // Add an existing property
      var property = internal_post.GetType().GetProperty("Text");
      custom_type.AddProperty(new CustomPropertyInfo(internal_post, property));

      // Add dependency
      internal_post.PropertyChanged += (sender, args) =>
      {
        if (args.PropertyName == "Text")
          NotifyPropertyChanged("ModifiedText");
      };

      // Create and add a new property
      //var expr_property = new ExpressionPropertyInfo<Post, string>("ModifiedText");
      //expr_property.internal_object = internal_post;
      //Expression<Func<Post, string>> expr = (p) => p.Text + " modified";
      //expr_property.GetFunc = expr.Compile();
      //custom_type.AddProperty(expr_property);
      var expr_property = new ExpressionPropertyInfo<string>("ModifiedText");
      Expression<Func<string>> expr = () => internal_post.Text + " modified";
      expr_property.GetFunc = expr.Compile();
      custom_type.AddProperty(expr_property);
    }

    private readonly CustomType custom_type = new CustomType(typeof(PostViewModel));
    public Type GetCustomType()
    {
      return custom_type;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void NotifyPropertyChanged([CallerMemberName] string property_name = "")
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(property_name));
    }
  }
}
