namespace DataWork.Web
{
    public class GlobalConstString
    {
        public const string Error = "error";
        public const string Success = "success";
        public const string Warning = "warning";

        public const string NotFound = "Record not found";
        public const string LoadError = "An error occurred while generating the data";
        public const string SaveError = "Error. The record has not been successfully saved. Please try again later";
        public const string SaveSuccess = "Success. Record saved successfully";
        public const string RequiredProjectMessage = "Please choоse valid project";
        public const string DeleteSuccess = "The record has been successfully deleted";
        public const string DeleteError = "Error. The record has not been successfully deleted. Please try again later";
        public const string ChangeSuccess = "The leave was successfully sent to approval";
        public const string ChangeError = "Error. The leave was not successfully submitted to approval. Please try again later";
        public const string ChangeHeadSuccess = "The leave was successfully {0}";
        public const string ChangeHeadError = "The leave was not successfully {0}";
        public const string AddDaysSuccess = "The leave days for {0} was successfully added";
        public const string AddDaysToAll = "The leave days for {0} was successfully added to all users who have not been set";
        public const string AddDaysError = "Error. The leave days was not successfully added. Please try again later";
        public const string AddDaysToAllNoUserFound = "No users were found without daily leave for the current year";


        public const string AdministratorAreaName = "Administrator";
        public const string AdministratorRole = "Administrator";

        public const string WorkerRole = "Worker";

        public const string ManagerRole = "Manager";
        public const string ManagerAreaName = "Manager";

        public const int IdNumberMinLength = 10;
        public const int IdNumberMaxLength = 50;

        public const string StartDateDisplayName = "Start Date";
        public const string EndDateDisplayName = "End Date";
        public const string TotalLeaveDaysDisplayName = "Total days";
        public const string LeaveTypeDisplayName = "Leave Type";

        public const string ControllerAdmin = "Admin";
        public const string ControllerWorks = "Works";
        public const string ControllerLeaves = "Leaves";
        public const string ControllerHeads = "Heads";
    }
}
