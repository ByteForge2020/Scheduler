import React, { useEffect } from 'react';
import { Scheduler } from "@aldabil/react-scheduler";
import { EVENTS } from '../EVENTS';
import { ApiClient } from '../api/api-client';


export default function SchedulerAppointments() {
    
    return (
        <Scheduler
            events={EVENTS}
            disableViewer
            fields={[
                {
                    name: "customDropdown",
                    type: "select",
                    // label: "Custom Dropdown", 
                    // required: true, 
                    options: [
                        { id: "option1", text: "option1", value: "option1" }
                    ],
                },
            ]}
            onEventClick={() => {
                const resp = ApiClient.get("/api/General/Test");
            }}                          
        />
    );
};