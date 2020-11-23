using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Octokit;

namespace GithubExplorer {
	public sealed class RetryRateLimitRunner {
		readonly ILogger _logger;

		public RetryRateLimitRunner(ILogger<RetryRateLimitRunner> logger) {
			_logger = logger;
		}

		public Task<T> Run<T>(Func<Task<T>> func) => Run(func, 3, TimeSpan.FromMinutes(1));

		public async Task<T> Run<T>(Func<Task<T>> func, int maxReties, TimeSpan maxDelay) {
			try {
				return await func();
			} catch ( RateLimitExceededException e ) {
				var now       = DateTimeOffset.UtcNow;
				var reset     = e.Reset;
				var nextDelay = reset - now;
				if ( nextDelay > maxDelay ) {
					_logger.LogError($"Rate limit reached, but next delay higher than maximum: {nextDelay}, exception: {e}");
					throw;
				}
				var nextRetries = maxReties - 1;
				_logger.LogWarning(
					$"Rate limit reached, wait for delay: {nextDelay}, retries left: {nextRetries}, exception: {e}");
				await Task.Delay(nextDelay);
				return await Run(func, nextRetries, maxDelay);
			}
		}
	}
}