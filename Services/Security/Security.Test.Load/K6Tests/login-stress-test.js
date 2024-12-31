import http from 'k6/http';
import { sleep } from 'k6';

// Test options
export const options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: createStages([1500], 1, 1, 1),
};

// Base URL and payload
const API_BASE_URL = 'https://localhost:7065';
const payload = JSON.stringify({
    Email: 'admin@admin.com',
    Password: 'Abcd@1234',
});
const headers = { 'Content-Type': 'application/json' };

// Helper function to generate stages
function createStages(targets, rampUpDuration, holdDuration, rampDownDuration) {
    const stages = [];
    targets.forEach((target) => {
        stages.push({ duration: `${rampUpDuration}m`, target });
        stages.push({ duration: `${holdDuration}m`, target });
    });
    stages.push({ duration: `${rampDownDuration}m`, target: 0 });
    return stages;
}

// Test execution
export default function () {
    const url = `${API_BASE_URL}/fa/api/v1/Auth/Login`; 
    http.post(url, payload, { headers });
    sleep(1);
}
