using EduHub.Domain;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("============================================");
Console.WriteLine("       EDUHUB - CONSOLE DEMO CH01");
Console.WriteLine("============================================\n");

// Bước 1: Tạo CLB
var club = Club.Create("CLB Công nghệ", "Cộng đồng học tập và chia sẻ công nghệ.");
Console.WriteLine($"BƯỚC 1 - Tạo CLB: {club.Name} | Id: {club.Id}");

// Bước 2: Thêm 3 thành viên
var members = new List<Member>
{
    Member.Create("Nguyễn An", "an@example.com", "SV001"),
    Member.Create("Trần Bình", "binh@example.com", "SV002"),
    Member.Create("Lê Chi", "chi@example.com", "SV003")
};
foreach (var member in members) club.AddMember(member);
Console.WriteLine($"BƯỚC 2 - Thêm {club.MemberCount()} thành viên vào CLB.");

// Bước 3: Tạo sự kiện
var ev = Event.Create(club.Id, "Workshop C# cơ bản", DateTime.Today.AddDays(7), capacity: 3, isOnline: false);
Console.WriteLine($"BƯỚC 3 - Tạo sự kiện: {ev.Title} | Sức chứa: {ev.Capacity} | Ngày: {ev.Date:dd/MM/yyyy}");

// Bước 4: Đăng ký (Pending)
var registrations = new List<Registration>();
for (var i = 0; i < members.Count; i++) registrations.Add(Registration.Create(members[i], ev, registrations.Count));
Console.WriteLine($"BƯỚC 4 - Đăng ký: tạo {registrations.Count} phiếu ở trạng thái Pending.");

// Bước 5: Xác nhận 2 phiếu, phiếu thứ 3 vẫn Pending
registrations[0].Confirm();
registrations[1].Confirm();
Console.WriteLine("BƯỚC 5 - Xác nhận: 2 phiếu Confirmed, 1 phiếu Pending.");

// Bước 6: Check-in 2 phiếu đã xác nhận
var checkIns = new List<CheckIn>
{
    CheckIn.Create(registrations[0], CheckInMethod.QR),
    CheckIn.Create(registrations[1], CheckInMethod.Manual)
};
Console.WriteLine($"BƯỚC 6 - Check-in: ghi nhận {checkIns.Count} lượt điểm danh.");

// Bước 7: Báo cáo tham dự
var confirmedCount = registrations.Count(r => r.Status == RegistrationStatus.Confirmed);
var checkedInRegistrationIds = checkIns.Select(c => c.RegistrationId).ToHashSet();
var checkedInCount = registrations.Count(r => checkedInRegistrationIds.Contains(r.Id));
Console.WriteLine("\nBƯỚC 7 - BÁO CÁO THAM DỰ");
Console.WriteLine($"Sự kiện: {ev.Title}");
Console.WriteLine($"Số đăng ký / sức chứa: {registrations.Count} / {ev.Capacity}");
Console.WriteLine($"Số phiếu đã xác nhận: {confirmedCount}");
Console.WriteLine($"Số đã check-in: {checkedInCount}");
Console.WriteLine("Chi tiết từng sinh viên:");
for (var i = 0; i < members.Count; i++)
{
    var status = registrations[i].Status;
    var checkin = checkedInRegistrationIds.Contains(registrations[i].Id) ? "Đã check-in" : "Chưa check-in";
    Console.WriteLine($"- {members[i].Name} ({members[i].StudentCode}) | Đăng ký: {status} | {checkin}");
}

Console.WriteLine("\nDEMO HOÀN TẤT.");