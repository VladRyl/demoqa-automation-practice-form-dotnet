# DemoQA browser tests

## Run with Docker

The container uses the Playwright .NET image matching the `Microsoft.Playwright` package version in the test project.

```bash
docker compose run --build --rm ui-tests
```

Test results are written to the host's `allure-results` directory. To build and open the HTML report locally, install the Allure CLI and run:

```bash
allure generate allure-results --output allure-report
allure open allure-report
```

## GitHub Actions

The `UI tests and Allure report` workflow uses Docker Compose to build and run the tests on GitHub Actions for pushes to `main`, pull requests, and manual dispatches. Each run uploads two artifacts:

- `allure-results` — raw test data
- `allure-report` — generated HTML report

Download either artifact from the completed workflow run in the repository's **Actions** tab. Runs on `main` also publish the HTML report to GitHub Pages, including when tests fail, so failures remain inspectable. Pull requests upload the artifacts but do not publish a Pages deployment.

Enable Pages once in the repository settings: open **Settings → Pages** and select **GitHub Actions** as the build and deployment source. After the first successful deployment, GitHub shows the report URL in the `github-pages` environment and the Actions deployment summary.
