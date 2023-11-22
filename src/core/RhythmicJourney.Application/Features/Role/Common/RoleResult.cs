using System.Threading.Tasks;
using System.Collections.Generic;
using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Application.Features.Role.Common
{
    /// <summary>
    /// Netice olaraq rolla elaqeli dondureceyimiz uygun neticeni ('Result Object Design Pattern'-in implementasiyasi olan) bu sinif vasitesile dondururuk.
    /// </summary>
    public partial class RoleResult
    {
        private RoleResult() { /* Awagidaki spesifik neticeleri temsil eden metodlarin cagirilmasini isteyirem deye bu konstruktoru 'private' vasitesile gizledirem */ }

        /// <summary>
        /// Netice olaraq rolun detallarini dondurmek isteyirikse bu overloadi iwledirik.
        /// </summary>
        public static Task<RoleResult> SuccessAsync(List<AppRole> roles) => Task.FromResult(new RoleResult() { Roles = roles, IsSuccess = true });

        /// <summary>
        /// Netice olaraq X bir rol daxilindeki userlerin detallarini dondurmek isteyirikse bu overloadi iwledirik.
        /// </summary>
        public static Task<RoleResult> SuccessAsync(List<AppUser> users) => Task.FromResult(new RoleResult() { Users = users, IsSuccess = true });

        /// <summary>
        /// Netice olaraq sadece mesaj dondurmek isteyirikse bu overloadi iwledirik.
        /// </summary>
        public static Task<RoleResult> SuccessAsync(string message) => Task.FromResult(new RoleResult() { Message = message, IsSuccess = true });

        /// <summary>
        /// Netice olaraq baw vermiw xeta haqqinda mesaj dondurmek isteyirikse bu overloadi iwledirik.
        /// </summary>
        public static Task<RoleResult> FailureAsync(string message) => Task.FromResult(new RoleResult() { Message = message, IsSuccess = false });

        /// <summary>
        /// Netice olaraq baw vermiw xeta haqqinda mesaj dondurmek isteyirikse bu overloadi iwledirik.
        /// </summary>
        public static Task<RoleResult> FailureAsync(List<string> errors) => Task.FromResult(new RoleResult() { Errors = errors, IsSuccess = false });
    }

    public partial class RoleResult
    {
        public List<AppRole> Roles { get; private set; }
        public List<AppUser> Users { get; private set; }

        public string Message { get; private set; }

        public bool IsSuccess { get; private set; }

        public List<string> Errors { get; private set; }
    }
}