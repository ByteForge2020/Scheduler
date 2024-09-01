import React from 'react';
import { Scheduler } from "@aldabil/react-scheduler";
import { EVENTS } from "./EVENTS";
import { ApiClient } from './api/api-client';

export default function SchedulerAppointments() {
    
    return (
        <Scheduler
            events={EVENTS}
            disableViewer
            fields={[
                {
                    name: "customDropdown",
                    type: "select",
                    // label: "Custom Dropdown", // Название поля
                    // required: true, // Делает поле обязательным
                    options: [
                        { id: "option1", text: "option1", value: "option1" }
                    ],
                },
                // Другие кастомные поля
            ]}
            onEventClick={() => {
                const resp = ApiClient.get("/api/General/Test");
            }}
        />
    );
};