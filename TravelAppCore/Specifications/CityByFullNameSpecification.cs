using System;
using System.Collections.Generic;
using System.Text;
using TravelAppCore.Entities;

namespace TravelAppCore.Specifications
{
    class CityByFullNameSpecification:BaseSpecification<City>
    {
        public CityByFullNameSpecification(string fullName): base(c => c.FullName == fullName){}
    }
}
