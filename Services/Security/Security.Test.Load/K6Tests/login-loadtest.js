import http from 'k6/http';
import { check } from 'k6';

const url = 'https://localhost:7065/fa/api/v1/Auth/Login';
const payload = JSON.stringify({
    Email: "admin@admin.com",
    Password: "Abcd@1234"
});

export default function () {
    const res = http.post(url, payload, {
        headers: { 'Content-Type': 'application/json' },
    });


    check(res, {
        'status is 200': (r) => r.status === 200,
        'response time is less than 200ms': (r) => r.timings.duration < 200,
    });
}
