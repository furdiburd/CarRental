using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Doga2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!nameValid())
                {
                    MessageBox.Show("A név nem lehet üres, legalább 3 karakter hosszúnak kell lennie.", "Hibás név", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!ageValidAndOldEnough())
                {
                    MessageBox.Show("Az életkor csak szám lehet, legalább 18 évesnek kell lennie és maximum 100 éves lehet.", "Hibás életkor", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!licenseValid())
                {
                    MessageBox.Show("A jogosítvány számnak legalább egy 6 számjegy hosszú számnak kell lennie.", "Hibás jogosítvány szám", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                if (!autoSelected())
                {
                    MessageBox.Show("Autó választása kötelező.", "Nincs autó kiválasztva", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!isRentLenghtValid())
                {
                    MessageBox.Show("A bérlés hossza csak szám lehet, legalább 1 napnak kell lennie és maximum 30 nap lehet.", "Hibás bérlés hossza", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!isInsuranceSelected())
                {
                    MessageBox.Show("Biztosítás kiválasztása kötelező.", "Nincs biztosítás kiválasztva", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!isPaymentMethodSelected())
                {
                    MessageBox.Show("Fizetési mód kiválasztása kötelező.", "Nincs fizetési mód kiválasztva", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!isTermsAccepted())
                {
                    MessageBox.Show("A feltételek elfogadása kötelező.", "Feltételek elfogadása szükséges", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                showOrder();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool nameValid()
        {
            string name = textName.Text.Trim();
            return !string.IsNullOrEmpty(name) && name.Length >= 3;
        }
        private bool ageValidAndOldEnough()
        {
            try
            {
                int age = int.Parse(textAge.Text.Trim());
                return age >= 18 && age <= 100;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
        }

        private bool licenseValid()
        {
            string license = textLicense.Text.Trim();
            return !string.IsNullOrEmpty(license) && license.Length >= 6 && int.TryParse(license, out _);

        }


        private bool autoSelected()
        {
            return comboCar.SelectedItem != null;
        }

        private bool isRentLenghtValid()
        {
            try
            {
                int rentLength = int.Parse(textDays.Text.Trim());
                return rentLength >= 1 && rentLength <= 30;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
        }

        private bool isInsuranceSelected()
        {
            return radioInsuranceBasic.IsChecked == true || radioInsuranceExtra.IsChecked == true || radioInsuranceFull.IsChecked == true;
        }

        private bool isPaymentMethodSelected()
        {
            return radioPaymentCash.IsChecked == true || radioPaymentCard.IsChecked == true;
        }

        private bool isTermsAccepted()
        {
            return checkTerms.IsChecked == true;
        }

        private string getSelectedInsurance()
        {
            if (radioInsuranceBasic.IsChecked == true)
            {
                return "Alap";
            }
            else if (radioInsuranceExtra.IsChecked == true)
            {
                return "Extra";
            }
            else if (radioInsuranceFull.IsChecked == true)
            {
                return "Teljes";
            }
            else
            {
                return null;
            }
        }

        private int getSelectedInsurancePrice()
        {
            if (radioInsuranceBasic.IsChecked == true)
            {
                return 0;
            }
            else if (radioInsuranceExtra.IsChecked == true)
            {
                return 3000;
            }
            else if (radioInsuranceFull.IsChecked == true)
            {
                return 6000;
            }
            else
            {
                return 0;
            }
        }

        private string getSelectedExtras()
        {
            StringBuilder extras = new StringBuilder();
            if (checkGPS.IsChecked == true)
            {
                extras.Append("GPS, ");
            }
            if (checkBabySeat.IsChecked == true)
            {
                extras.Append("Gyerekülés, ");
            }
            if (checkAdditionalDriver.IsChecked == true)
            {
                extras.Append("További sofőr, ");
            }
            if (checkHighwayVignette.IsChecked == true)
            {
                extras.Append("Autópálya matrica, ");
            }
            if (extras.Length > 0)
            {
                extras.Length -= 2;
            }
            else
            {
                extras.Append("Nincs");
            }
            return extras.ToString();
        }

        private string getSelectedPaymentMethod()
        {
            if (radioPaymentCash.IsChecked == true)
            {
                return "Készpénz";
            }
            else if (radioPaymentCard.IsChecked == true)
            {
                return "Bankkártya";
            }
            else
            {
                return null;
            }
        }

        private string getSelectedCarName()
        {
            if (comboCar.SelectedItem == null) return null;
            return ((ComboBoxItem)comboCar.SelectedItem).Tag?.ToString();
        }

        private int getSelectedCarPrice()
        {
            switch (getSelectedCarName())
            {
                case "Suzuki Swift": return 12000;
                case "Toyota Corolla": return 18000;
                case "BMW 3": return 30000;
                case "Tesla Model 3": return 35000;
                default: return 0;
            }
        }


        private int getSelectedExtrasPrice()
        {
            int extrasPrice = 0;
            if (checkGPS.IsChecked == true)
            {
                extrasPrice += 1000 * int.Parse(textDays.Text.Trim());
            }
            if (checkBabySeat.IsChecked == true)
            {
                extrasPrice += 1500 * int.Parse(textDays.Text.Trim());
            }
            if (checkAdditionalDriver.IsChecked == true)
            {
                extrasPrice += 2500 * int.Parse(textDays.Text.Trim());
            }
            if (checkHighwayVignette.IsChecked == true)
            {
                extrasPrice += 5000;
            }
            return extrasPrice;
        }

        private string calculateTotalPrice()
        {
            int totalPrice = 0;
            totalPrice += getSelectedCarPrice() * int.Parse(textDays.Text.Trim());
            totalPrice += getSelectedInsurancePrice() * int.Parse(textDays.Text.Trim());
            totalPrice += getSelectedExtrasPrice();
            return totalPrice.ToString("N0");
;
        }

        private void showOrder()
        {
            string message =
                "Sikeres autóbérlés!\n\n" +
                $"Ügyfél: {textName.Text.Trim()}\n" +
                $"Autó: {((ComboBoxItem)comboCar.SelectedItem).Tag.ToString()}\n" +
                $"Időtartam: {textDays.Text.Trim()} nap\n" +
                $"Biztosítás: {getSelectedInsurance()}\n" +
                $"Extrák: {getSelectedExtras()}\n" +
                $"Fizetés: {getSelectedPaymentMethod()}\n\n"+
                $"Fizetendő: {calculateTotalPrice()} Ft";

            MessageBox.Show(message, "Sikeres foglalás", MessageBoxButton.OK);
        }
    }
}