
using Makale.DataAccessLayer.EF;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class Test
    {
        private Repository<Category> repo_category = new Repository<Category>();
        private Repository<User> repo_user = new Repository<User>();
        private Repository<Comment> repo_comment = new Repository<Comment>();
        private Repository<Note> repo_note = new Repository<Note>();



        public Test()
        {
            
            List<Category> categories = repo_category.List();
        }

        public void Insert()
        {
          
            int result= repo_user.Insert(new User
            {
                Name = "aaaa",
                Surname = "bbbb",
                Email = "ccc@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "aaa",
                Password = "111",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUsername = "ccdd"
            });

        }
        public void Update()
        {
            User user=repo_user.Find(m => m.Username == "aaa");
            if(user !=null)
            {
            user.Username = "asasasas";
            int a = repo_user.Update(user);

            }


        }
        public void Delete()
        {
            User user = repo_user.Find(m => m.Username == "asasasas");
            if (user != null)
            {
                int test = repo_user.Delete(user);
            }
            
        }

        public void CommentTest()
        {
            User user=repo_user.Find(x => x.Id == 1);
            Note note = repo_note.Find(x => x.Id == 3);

            Comment comment = new Comment()
            {
                Text="test yorumu",
                ModifiedUsername="firatc",
                CreatedOn = DateTime.Now,
                ModifiedOn= DateTime.Now,
                Note=note,
                Owner=user,

            };

            repo_comment.Insert(comment);
        }
    }
}
