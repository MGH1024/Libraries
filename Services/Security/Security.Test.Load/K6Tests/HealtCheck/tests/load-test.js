import http from 'k6/http';
import { check, sleep } from 'k6';

// Load test configuration
export let options = {
    stages: [
        { duration: '30s', target: 10 },  // Ramp up to 10 users in 30 seconds
        { duration: '1m', target: 10 },   // Hold at 10 users for 1 minute
        { duration: '30s', target: 0 },  // Ramp down to 0 users in 30 seconds
    ],
};

const API_BASE_URL = 'https://localhost:7065';

export default function () {
    const url = `${API_BASE_URL}/healthchecks-api`;
    const res = http.get(url);
    
    check(res, {
        'is status 200': (r) => r.status === 200,
        'response time < 200ms': (r) => r.timings.duration < 200,
    });
    
    sleep(1);
}
