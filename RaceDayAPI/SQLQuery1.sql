UPDATE dbo.Users
SET Role = 'Organiser'
WHERE Email = 'Reggy@05';

SELECT UserID, Email, Role
FROM dbo.Users
WHERE Email = 'Reggy@05'