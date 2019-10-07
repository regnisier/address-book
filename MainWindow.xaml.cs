using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace AddressBook
{
    public partial class MainWindow : Window
    {
        string fileName = "SavedContacts.txt";

        List<ContactInformation> contactList = new List<ContactInformation>();

        enum orderTypes
        {
            firstNameAsc, firstNameDesc, secondNameAsc, secondNameDesc,
            telephoneAsc, telephoneDesc, eMailAsc, eMailDesc
        };

        orderTypes orderContactsyBy = orderTypes.firstNameAsc;

        public MainWindow()
        {
            InitializeComponent();

            CreateFile();

            LoadContacts();
        }

        public void CreateFile()
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
        }

        // Loads contacts from file into the listview. 
        public void LoadContacts()
        {
            contactList.Clear();

            string line = "";

            try
            {
                using (StreamReader file = new StreamReader(fileName))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] splits = line.Split(',');

                        ContactInformation currentContact = new ContactInformation(
                                splits[0], splits[1], splits[2], splits[3]
                            );

                        contactList.Add(currentContact);
                    }
                }
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

            // Order contacts based on which column header has been clicked.
            switch (orderContactsyBy)
            {
                case orderTypes.firstNameAsc:
                    contactList = contactList.OrderBy(x => x.firstName).ToList();
                    break;
                case orderTypes.firstNameDesc:
                    contactList = contactList.OrderByDescending(x => x.firstName).ToList();
                    break;
                case orderTypes.secondNameAsc:
                    contactList = contactList.OrderBy(x => x.secondName).ToList();
                    break;
                case orderTypes.secondNameDesc:
                    contactList = contactList.OrderByDescending(x => x.secondName).ToList();
                    break;
                case orderTypes.telephoneAsc:
                    contactList = contactList.OrderBy(x => x.telephoneNumber).ToList();
                    break;
                case orderTypes.telephoneDesc:
                    contactList = contactList.OrderByDescending(x => x.telephoneNumber).ToList();
                    break;
                case orderTypes.eMailAsc:
                    contactList = contactList.OrderBy(x => x.eMail).ToList();
                    break;
                case orderTypes.eMailDesc:
                    contactList = contactList.OrderByDescending(x => x.eMail).ToList();
                    break;
                default:
                    break;
            }

            addressEntriesListView.ItemsSource = contactList;
        }

        // Creates the form that lets the user create a new contact.
        private void ButtonNewContact_Click(object sender, RoutedEventArgs e)
        {
            Window newContactWindow = new newContactWindow(fileName);
            newContactWindow.Show();
        }

        // Deletes an existing contact. 
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            string line = "";
            string tmpFileName = "tmp.txt";

            ContactInformation selectedEntry = (ContactInformation)addressEntriesListView.SelectedItem;

            if (selectedEntry != null)
            {
                DialogResult result = MessageBox.Show(String.Format("Do you really want to delete the " +
                    "contact entry of {0} {1}?", selectedEntry.firstName, selectedEntry.secondName),
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        // To read from saved contacts file.
                        using (StreamReader sr = new StreamReader(fileName))
                        {
                            // To copy the file line for line without the line to delete.
                            using (StreamWriter sw = new StreamWriter(tmpFileName))
                            {
                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (line == selectedEntry.ToString())
                                    {
                                        // Do not write line.
                                    }
                                    else
                                    {
                                        sw.WriteLine(line);
                                    }
                                }
                            }
                        }
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

                    // Delete the old file and replace it with the updated one.
                    File.Delete(fileName);
                    File.Move(tmpFileName, fileName);

                    LoadContacts();
                }
                else
                {
                    // Do nothing.
                }

            }
            else
            {
                MessageBox.Show("Please choose the entry you want to delete first.", "Reminder",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Creates a form that lets the user edit a selected contact.
        private void ButtonEditContact_Click(object sender, RoutedEventArgs e)
        {
            if (addressEntriesListView.SelectedItem != null)
            {
                ContactInformation originalEntry = (ContactInformation)addressEntriesListView.SelectedItem;

                EditContactWindow editContactWindow = new EditContactWindow(fileName, originalEntry);
                editContactWindow.Show();
            }
            else
            {
                MessageBox.Show("Please choose the entry you want to edit first.", "Reminder",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Used to refresh the contacts after a new contact has been created or an existing contact has been edited. 
        private void Window_Activated(object sender, EventArgs e)
        {
            LoadContacts();
        }

        // Sorts the contacts depending on which column header has been clicked.
        private void addressEntriesListViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader columnHeader = sender as GridViewColumnHeader;
            var columnName = columnHeader.Tag.ToString();

            switch (columnName)
            {
                case "FirstName":
                    if (orderContactsyBy == orderTypes.firstNameAsc)
                    {
                        orderContactsyBy = orderTypes.firstNameDesc;
                    }
                    else
                    {
                        orderContactsyBy = orderTypes.firstNameAsc;
                    }
                    LoadContacts();
                    break;
                case "SecondName":
                    if (orderContactsyBy == orderTypes.secondNameAsc)
                    {
                        orderContactsyBy = orderTypes.secondNameDesc;
                    }
                    else
                    {
                        orderContactsyBy = orderTypes.secondNameAsc;
                    }
                    LoadContacts();
                    break;
                case "TelephoneNumber":
                    if (orderContactsyBy == orderTypes.telephoneAsc)
                    {
                        orderContactsyBy = orderTypes.telephoneDesc;
                    }
                    else
                    {
                        orderContactsyBy = orderTypes.telephoneAsc;
                    }
                    LoadContacts();
                    break;
                case "EMailAddress":
                    if (orderContactsyBy == orderTypes.eMailAsc)
                    {
                        orderContactsyBy = orderTypes.eMailDesc;
                    }
                    else
                    {
                        orderContactsyBy = orderTypes.eMailAsc;
                    }
                    LoadContacts();
                    break;
                default:
                    // Do nothing.
                    break;
            }
        }
    }
}
