namespace MOE.Common.Models.Repositories
{
    public class MenuRepositoryFactory
    {
        private static IMenuRepository menuRepository;

        public static IMenuRepository Create()
        {
            if (menuRepository != null)
                return menuRepository;
            return new MenuRepository();
        }

        public static void SetMenuRepository(IMenuRepository newRepository)
        {
            menuRepository = newRepository;
        }
    }
}