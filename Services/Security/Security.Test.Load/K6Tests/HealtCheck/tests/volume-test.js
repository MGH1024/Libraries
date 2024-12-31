import http from 'k6/http';
import { check } from 'k6';

// Volume test configuration
export let options = {
    stages: [
        { duration: '1m', target: 50 },  // Ramp up to 50 users in 1 minute
        { duration: '10m', target: 50 }, // Sustain 50 users for 10 minutes
        { duration: '1m', target: 0 },   // Ramp down to 0 users in 1 minute
    ],
    thresholds: {
        http_req_duration: ['p(95)<300'], // 95% of requests should complete under 300ms
        http_req_failed: ['rate<0.01'],  // Less than 1% of requests should fail
    },
};

const API_BASE_URL = 'https://localhost:7065';
export default function () {
    const url = `${API_BASE_URL}/healthchecks-api`;
    const res = http.get(url);

    // Validate the response
    check(res, {
        'status is 200': (r) => r.status === 200,
        'response time < 300ms': (r) => r.timings.duration < 300,
    });
}
