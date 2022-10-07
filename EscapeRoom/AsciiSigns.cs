using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    internal class AsciiSigns
    {
        // Link to website: http://patorjk.com/software/taag/#p=display&f=Graceful&t=Type%20Something

        public string menuSign = @"
  _  _  ____  __ _  _  _ 
 ( \/ )(  __)(  ( \/ )( \
 / \/ \ ) _) /    /) \/ (
 \_)(_/(____)\_)__)\____/
";

        public string pauseSign = @"
 ____   __   _  _  ____  ____ 
(  _ \ / _\ / )( \/ ___)(  __)
 ) __//    \) \/ (\___ \ ) _) 
(__)  \_/\_/\____/(____/(____)
";

        public string gameExplanationSign = @"
  ____  ____  __  ____  __     __   ____  __     __   _  _  ____ 
 / ___)(  _ \(  )(  __)(  )   / _\ (  _ \(  )   / _\ / )( \(  __)
 \___ \ ) __/ )(  ) _) / (_/\/    \ ) _ (/ (_/\/    \) \/ ( ) _) 
 (____/(__)  (__)(____)\____/\_/\_/(____/\____/\_/\_/\____/(__)  
";

        public string roomCreatorSign = @"
  ____   __    __   _  _   ___  ____  ____   __  ____  __  ____ 
 (  _ \ /  \  /  \ ( \/ ) / __)(  _ \(  __) / _\(_  _)/  \(  _ \
  )   /(  O )(  O )/ \/ \( (__  )   / ) _) /    \ )( (  O ))   /
 (__\_) \__/  \__/ \_)(_/ \___)(__\_)(____)\_/\_/(__) \__/(__\_)
";

        public string endSign = @"
  ____  __ _  ____  ____ 
 (  __)(  ( \(    \(  __)
  ) _) /    / ) D ( ) _) 
 (____)\_)__)(____/(____)
";

        public string startSign = @"
  ____  ____  __   ____  ____ 
 / ___)(_  _)/ _\ (  _ \(_  _)
 \___ \  )( /    \ )   /  )(  
 (____/ (__)\_/\_/(__\_) (__) 
";
        
        public string errorSign = @"
 (`-')  _   (`-')    (`-')               (`-')  
 ( OO).-/<-.(OO ) <-.(OO )      .->   <-.(OO )  
(,------.,------,),------,)(`-')----. ,------,) 
 |  .---'|   /`. '|   /`. '( OO).-.  '|   /`. ' 
(|  '--. |  |_.' ||  |_.' |( _) | |  ||  |_.' | 
 |  .--' |  .   .'|  .   .' \|  |)|  ||  .   .' 
 |  `---.|  |\  \ |  |\  \   '  '-'  '|  |\  \  
 `------'`--' '--'`--' '--'   `-----' `--' '--' 
";
    }
}
