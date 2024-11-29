import http from 'k6/http';
import { check, group } from 'k6';

// Compliance test configuration
export let options = {
    stages: [
        { duration: '1m', target: 5 },   // Ramp up to 5 users over 1 minute
        { duration: '5m', target: 5 },   // Maintain 5 users for 5 minutes
        { duration: '1m', target: 0 },   // Ramp down to 0 users over 1 minute
    ],
    thresholds: {
        http_req_failed: ['rate<0.01'],  // Less than 1% of requests should fail
    },
};


const API_BASE_URL = 'https://localhost:7065';
export default function () {
    const url = `${API_BASE_URL}/healthchecks-api`;

    group('Check Compliance for /healthchecks-api', () => {
        const res = http.get(url);

        // Validate the response status code
        check(res, {
            'status is 200': (r) => r.status === 200,
            'content type is application/json': (r) => r.headers['Content-Type'] === 'application/json',
            'response contains expected health status': (r) => r.body.includes('healthy'), // Example for health check text
        });

        // Example of validating security headers
        check(res, {
            'has X-Content-Type-Options header': (r) => r.headers['X-Content-Type-Options'] === 'nosniff',
            'has X-Frame-Options header': (r) => r.headers['X-Frame-Options'] === 'DENY',
        });
    });
}
