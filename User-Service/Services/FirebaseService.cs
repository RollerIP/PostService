using FirebaseAdmin.Auth;
using User_Service.Contexts;
using User_Service.Messaging;
using User_Service.Models;

namespace User_Service.Services
{
    public class FirebaseService
    {
        private readonly IMessageService messageService;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly FirebaseAuth auth;

        public FirebaseService(IMessageService messageService, IServiceScopeFactory scopeFactory)
        {
            this.messageService = messageService;
            this.scopeFactory = scopeFactory;
            auth = FirebaseAuth.DefaultInstance;
            SyncUserDatabase();
        }

        private async void SyncUserDatabase()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();
                if (context.Users.Count() > 0)
                {
                    return;
                }

                var pagedEnumerable = FirebaseAuth.DefaultInstance.ListUsersAsync(null);
                var responses = pagedEnumerable.AsRawResponses().GetAsyncEnumerator();

                while (await responses.MoveNextAsync())
                {
                    List<User> updatedUsers = new List<User>();

                    ExportedUserRecords response = responses.Current;
                    foreach (ExportedUserRecord user in response.Users)
                    {
                        User internalUser = new User(user.Uid, user.Email, user.DisplayName);

                        using (context)
                        {
                            context.Users.Add(internalUser);
                            context.SaveChanges();
                        }
                        updatedUsers.Add(internalUser);
                    }

                    BroadcastUpdate(updatedUsers);
                }
            }
        }

        private void BroadcastUpdate(List<User> updatedUsers)
        {
            messageService.Publish("UserUpdate", updatedUsers);
        }
    }
}
