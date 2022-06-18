using Makale.DataAccessLayer.EF;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class NoteManager
    {
        private Repository<Note> _repo_note = new Repository<Note>();

        public List<Note> GetAllNotes()
        {
            return _repo_note.List();
        }

    }
}
