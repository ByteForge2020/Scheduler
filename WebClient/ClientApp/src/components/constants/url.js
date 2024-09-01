"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Url = void 0;
exports.Url = {
    Default: "/",
    Authentication: {
        Login: "/login",
        Recovery: "/recovery",
        ResetPassword: "/resetpassword/:email/*",
        Logout: "/logout",
        Join: "/join/:email/*"
    },
    Appointments: {
        Main: "/appointments"
    },
    Customers: {
        Main: "/customers"
    },
    FollowUps: {
        Main: "/followups"
    },
    WhatsApp: {
        Main: "/whatsapp",
        MassiveSendings: "/massive-sendings"
    },
    Reports: {
        Main: "/reports",
        CustomReport: "/custom-report"
    },
    Settings: {
        Main: "/settings",
        CustomerCommunications: "/customer-communications",
        WorkAreas: "/work-areas",
        Services: "/services",
        Users: "/users",
        Payments: "/payments",
        ClinicHistory: "/clinic-history",
        EvolutionNotes: "/evolution-notes",
        PatientForm: "/patient-form",
        Commissions: "/commissions"
    },
    SignalRHubs: {
        CommonHub: "/commonHub",
        WhatsAppHub: "/whatsAppHub"
    },
    EstablishmentSettings: {
        ClinicHistory: "/clinic-history",
        EvolutionNotes: "/evolution-notes",
        PatientForm: "/patient-form"
    },
};
