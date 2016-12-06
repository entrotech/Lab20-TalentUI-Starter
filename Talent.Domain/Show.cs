using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UclaExt.Common.BaseClasses;
using UclaExt.Common.Interfaces;

namespace Talent.Domain
{
    public class Show : DomainBase, IDisplayable
    {
        #region Constructor

        #endregion

        #region Fields

        private int _id;
        private string _title;
        private int? _lengthInMinutes;
        private DateTime? _releaseDate;
        private int? _mpaaRatingId;
        private ObservableCollection<ShowGenre> _showGenres = new ObservableCollection<ShowGenre>();
        private ObservableCollection<Credit> _credits = new ObservableCollection<Credit>();

        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        public int? LengthInMinutes
        {
            get { return _lengthInMinutes; }
            set
            {
                if (_lengthInMinutes == value) return;
                _lengthInMinutes = value;
                OnPropertyChanged();
            }
        }

        public DateTime? ReleaseDate
        {
            get { return _releaseDate; }
            set
            {
                if (_releaseDate == value) return;
                _releaseDate = value;
                OnPropertyChanged();
            }
        }

        public int? MpaaRatingId
        {
            get { return _mpaaRatingId; }
            set
            {
                if (_mpaaRatingId == value) return;
                _mpaaRatingId = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ShowGenre> ShowGenres
        {
            get { return _showGenres; }
        }

        public ObservableCollection<Credit> Credits
        {
            get { return _credits; }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return Title;
        }

        public override bool GetHasChanges()
        {
            return base.GetHasChanges()
                || Credits.Any(o => o.IsDirty
                    || (o.Id == 0 && !o.IsMarkedForDeletion)
                    || (o.Id > 0 && o.IsMarkedForDeletion))
                || ShowGenres.Any(o => o.IsDirty
                    || (o.Id == 0 && !o.IsMarkedForDeletion)
                    || (o.Id > 0 && o.IsMarkedForDeletion));
        }


        #endregion

        #region IDisplayable

        public string Display()
        {
            string msg = Title ?? "";
            if(ReleaseDate.HasValue)
            {
                msg += "(" + ReleaseDate.Value.Year + ")";
            }
            return msg;
        }

        #endregion
    }

}
