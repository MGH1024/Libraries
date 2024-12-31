import http from 'k6/http';
import { check } from 'k6';

// Stress test configuration
export let options = {
    stages: [
        { duration: '1m', target: 20 },   // Ramp up to 20 users in 1 minute
        { duration: '2m', target: 50 },  // Increase to 50 users over 2 minutes
        { duration: '3m', target: 100 }, // Ramp up to 100 users over 3 minutes
        { duration: '2m', target: 100 }, // Hold at 100 users for 2 minutes
        { duration: '1m', target: 0 },   // Ramp down to 0 users in 1 minute
    ],
};

const API_BASE_URL = 'https://localhost:7065';
export default function () {
    const url = `${API_BASE_URL}/healthchecks-api`;
    const res = http.get(url);

    // Validate the response
    check(res, {
        'status is 200': (r) => r.status === 200,
        'response time < 500ms': (r) => r.timings.duration < 500,
    });
}
