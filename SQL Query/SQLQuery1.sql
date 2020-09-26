select c.CollegeName,d.Location from College c
inner join DistrictCollege dc on c.Id = dc.CollegeId
inner join District d on d.Id = dc.DistrictId;
