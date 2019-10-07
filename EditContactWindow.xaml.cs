using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace AddressBook
{
    public partial class EditContactWindow : Window
    {
        string fileName = "";
        ContactInformation contactToEdit;

        public EditContactWindow(string fileName_, ContactInformation contactToEdit_)
        {
            InitializeComponent();

            fileName = fileName_;
            contactToEdit = contactToEdit_;

            newFirstNameEntry.Text = contactToEdit.firstName;
            newSecondNameEntry.Text = contactToEdit.secondName;
            newEMailAddressEntry.Text = contactToEdit.eMail;
            newTelephoneNumberEntry.Text = contactToEdit.telephoneNumber;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Saves the changes.
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if ((newFirstNameEntry.Text.Length == 0) && newSecondNameEntry.Text.Length == 0)
            {
                MessageBox.Show("Please specify at least a first name or a second name before saving.", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if ((newTelephoneNumberEntry.Text.Length == 0) && newEMailAddressEntry.Text.Length == 0)
            {
                MessageBox.Show("A corresponding telephone number or e-mail address is needed in order " +
                    "to create a meaningful entry.", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ContactInformation UpdatedContactInformation = new ContactInformation(
                newFirstNameEntry.Text,
                newSecondNameEntry.Text,
                newTelephoneNumberEntry.Text,
                newEMailAddressEntry.Text
                );

            try
            {
                string line = "";
                string tmpFileName = "tmp.txt";

                if (contactToEdit != null)
                {
                    // To read from saved contacts file.
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        // To copy the file line for line except of the line to edit.
                        using (StreamWriter sw = new StreamWriter(tmpFileName))
                        {
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (line == contactToEdit.ToString())
                                {
                                    sw.WriteLine(UpdatedContactInformation.ToString());
                                }
                                else
                                {
                                    sw.WriteLine(line);
                                }
                            }
                        }
                    }

                    // Delete the old file and replace it with the updated one.
                    File.Delete(fileName);
                    File.Move(tmpFileName, fileName);
                }

                MessageBox.Show("The editing was successfull.", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.None);

                this.Close();
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("File not found!", "Error!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                MessageBox.Show("Something went wrong!", "Error!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
