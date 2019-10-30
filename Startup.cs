using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HaloweenHeist.Startup))]
namespace HaloweenHeist
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            RandomHolder.Random = new System.Random();
            var first = RandomHolder.Random.Next(100);
        }
    }
}
