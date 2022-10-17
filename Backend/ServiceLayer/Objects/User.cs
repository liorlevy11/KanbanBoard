namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct User
    {
        public readonly string Email;

        internal User(string email)
        {
            this.Email = email;
        }
    }
}
