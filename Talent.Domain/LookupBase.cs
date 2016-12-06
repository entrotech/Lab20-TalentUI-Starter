using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UclaExt.Common.BaseClasses;
using UclaExt.Common.Interfaces;

namespace Talent.Domain
{
    /// <summary>
    /// Base class for various "lookup table" type classes.
    /// </summary>
    public abstract class LookupBase : DomainBase, IDisplayable
    {

        #region Fields

        private int _id;
        private string _name;
        private string _code;
        private bool _isInactive;
        private int _displayOrder = 10;

        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
                
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                if (_code == value) return;
                _code = value;
                OnPropertyChanged();
            }
        }

        public bool IsInactive
        {
            get { return _isInactive; }
            set
            {
                if (_isInactive == value) return;
                _isInactive = value;
                OnPropertyChanged();
            }
        }

        public int DisplayOrder
        {
            get { return _displayOrder; }
            set
            {
                if (_displayOrder == value) return;
                _displayOrder = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region IDisplayable

        public virtual string Display()
        {
            return (Code ?? "") + " - " 
                + (Name ?? "");
        }

        #endregion

    }
}
