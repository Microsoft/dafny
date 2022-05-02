// Check for a successful previous run of a workflow.
// This could make a decent reusable action itself.
module.exports = async ({github, context, core}, workflowID, branch, sha = null) => {
  // This API doesn't support filtering by SHA, so we just
  // fetch the first page and scan manually.
  // That means if the run is fairly old it may be missed,
  // but that should be rare.
  const result = await github.rest.actions.listWorkflowRuns({
    owner: context.repo.owner,
    repo: context.repo.repo,
    branch,
    workflow_id: workflowID,

  })
  // These are ordered by creation time, so decide based on the first
  // run for this SHA we see.
  const runFilterDesc = sha ? `${workflowID} on ${sha}` : workflowID
  for (const run of result.data.workflow_runs) {
    if (sha == null || run.sha == sha) {
      if (run.conclusion != "success") {
        core.setFailed(`Last run of ${runFilterDesc} did not succeed: ${run.html_url}`)
      } else {
        // The SHA is fully-tested, exit with success
        console.log(`Found successful run of ${runFilterDesc}: ${run.html_url}`)
        return
      }
    }
  }
  core.setFailed(`No runs of ${runFilterDesc} found!`)
}
