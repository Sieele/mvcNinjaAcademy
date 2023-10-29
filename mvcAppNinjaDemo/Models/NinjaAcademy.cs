using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcAppNinjaDemo.Models
{
    public class Ninja
    {
        //-------------------NINJAS-------------------//
        // NinjaId, FirstName, LastName, DateOfBirth, Jutsu

        public int NinjaId { get; set; }

        [Required(ErrorMessage = "The First Name is Required"), Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The Last Name is Required"), Display(Name ="Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is Required"), Display(Name ="Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string? Jutsu { get; set; }

        //-------------------AddedINFO-------------------//

        public string? NinjaImagePath { get; set; }

        public bool? IsAlive { get; set; }

        public string? Information { get; set; }

        //-------------------RELATIONSHIP-------------------//

        //---TEAM---//
        public int? TeamId { get; set; } //Precisa do ? para o Create Funcionar
        public virtual Team? Team { get; set; } //Precisa do ? para o Create Funcionar

        //---IMAGE---//
        public int? ImageId { get; set; }
        public virtual Image? Image { get; set; }


        //-------------------DETAILS-------------------//

        [NotMapped, Display(Name ="Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }



    //-------------------TEAM-------------------//
    // TeamId, TeamName

    public class Team
    {
        public int TeamId { get; set; }

        [Required(ErrorMessage = "The Team Name is Required"), Display(Name ="Team Name")]
        public string TeamName { get; set; }

        //-------------------RELATIONSHIP-------------------//

        public virtual ICollection<Ninja>? Ninjas { get; set; } // Sem esse ?, o Team nao é adcionado

        public virtual Mission? Mission { get; set; } // Sem esse ?, o Team nao é adcionado
    }



    //-------------------MISSIONS-------------------//
    // MissionId, Rank, Local

    public class Mission
    {
        public int MissionId { get; set; }

        [Required(ErrorMessage = "Mission Rank is Required")]
        public string Rank { get; set; }

        [Required(ErrorMessage = "Missioin Local is Required")]
        public string Local { get; set; }

        //-------------------RELATIONSHIP-------------------//

        public int? TeamId { get; set; }

        public virtual Team? Team { get; set; }
       
    }



    //-------------------IMAGE-------------------//
    // ImageId, ImagePath, NinjaId

    public class Image
    {
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }

        public int? NinjaId { get; set; }

        public virtual ICollection<Ninja>? Ninjas { get; set; }
    }




}
