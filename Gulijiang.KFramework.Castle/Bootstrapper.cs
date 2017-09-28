using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;

namespace Gulijiang.KFramework.Castle
{
    /// <summary>
    /// Responsible for initializing the CMS application.
    /// </summary>
    public static class Bootstrapper
    {
        private static bool isLoad = false;
        /// <summary>
        /// 初始化
        /// </summary>
        /// 
        public static void InitializeContainer()
        {
            //防止多次加载
            if (isLoad == false)
            {
                // Initialize Windsor
                IWindsorContainer container = new WindsorContainer(new XmlInterpreter());

                // Inititialize the static Windsor helper class. 
                CastleIocHelper.Initialize(container);

                //初始化模块
             /////   ModuleManager.Initialize();

                isLoad = true;
            }

         
        }


    }
}
