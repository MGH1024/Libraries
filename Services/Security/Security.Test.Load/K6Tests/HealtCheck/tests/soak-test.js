import http from 'k6/http';
import { check, sleep } from 'k6';

// Soak test configuration
export let options = {
    stages: [
        { duration: '2m', target: 20 },  // Ramp up to 20 users over 2 minutes
        { duration: '4h', target: 20 }, // Sustain 20 users for 4 hours
        { duration: '2m', target: 0 },  // Ramp down to 0 users over 2 minutes
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'], // 95% of requests should complete under 500ms
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
        'response time < 500ms': (r) => r.timings.duration < 500,
    });

    // Pause briefly between requests
    sleep(1);
}
