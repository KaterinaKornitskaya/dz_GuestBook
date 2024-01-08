using dz_GuestBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Experimental;
using System.Security.Cryptography;
using System.Text;

namespace dz_GuestBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly GuestBookContext _context;

        public AccountController(GuestBookContext context)
        {
            _context=context;
        }
        
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel logon)
        {
            if (ModelState.IsValid)
            {
                // проверим, что пользователь существует
                if (_context.Users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                // проверим, есть ли искомый логин
                var users = _context.Users.Where(a => a.Login == logon.Login);
                if (users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                // если логин правильный:
                // получаем этого пользователя из БД
                var user = users.First();
                // получаем соль
                string? salt = user.Salt;

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);

                //создаем объект для получения средств шифрования  
                var md5 = MD5.Create();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = md5.ComputeHash(password);

                // переводим массив в строку
                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                // берем из БД захешированный пароль, и сравниваем с 
                // только что захешированным паролем
                if (user.Password != hash.ToString())
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                // дальше хотим воспользоваться сессией - тоесть пока мы работаем
                // с сайтом, нам не придется заново вводть логин и пароль
                HttpContext.Session.SetString("Login", user.Login);
                // переадресация на страницу Index контроллера HomeController
                return RedirectToAction("Index", "Home");
            }
            return View(logon);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel reg, User user)
        {
            if (LoginExist(user.Login))
            {
                ModelState.AddModelError("", "Такой логин уже существует!");
            }
            if (ModelState.IsValid)
            {
               
                //// создаем объект доменнной модели User
                //user = new User();
                //// заполняем имя, фамилия, логин
                //user.Name = reg.Name;
                //user.Login = reg.Login;

                // дальне работа с паролем:

                // создаем байтовый массив
                byte[] saltbuf = new byte[16];

                // создаем объект типа RandomNumberGenerator с помощью фабрики
                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                // генерируем байтовый массив
                randomNumberGenerator.GetBytes(saltbuf);
                // теперь нужно наш байтовый массив перевести в строку
                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                // выше добавили каждый элемент байтовго массива в объект 
                // типа StringBuilder, и преобразовали StringBuilder в строку
                string salt = sb.ToString();

                // пароль и соль конкатенируем, переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);

                //создаем объект типа MD5 для получения средств шифрования
                // (нужно подключить библиотеку System.Security.Cryptography)
                var md5 = MD5.Create();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = md5.ComputeHash(password);
                // в строке выше произошло хеширование массива, в котором
                // уже находится пароль и соль

                // переводим хеш-предствление в строку
                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                // заполняем пароль
                user.Password = hash.ToString();
                // заполняем соль
                user.Salt = salt;

                // добавляем пользователя в БД и сохраняем изменения в БД
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(reg);
        }

        public bool LoginExist(string login)
        {
            return _context.Users.Any(x => x.Login == login);
        }

    }
}
